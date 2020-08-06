using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json;

namespace Util_.JsonHelper_
{
	public static class JsonHelper
	{
		#region 생성자
		static JsonHelper()
		{
			// IGetConverter 을 상속 받은 클래스들의 인스턴스를 JsonConverter 타입으로 생성
			IEnumerable<JsonConverter> jsonConverters = Assembly.GetAssembly(typeof(JsonHelper)).GetTypes()
				.Where(type => typeof(IGetConverter).IsAssignableFrom(type) && !typeof(IGetConverter).Equals(type))
				.Select(type =>
				{
					try
					{
						return Activator.CreateInstance(type) as JsonConverter;
					}
					catch (Exception exception)
					{
						Debug.LogErrorFormat("Can't create JsonConverter {0}:\n{1}", type, exception);
					}
					return null;
				});
			
			// .net json 으로 세이브/로드가 비효율 적인 unity 구조체를 커스텀하게 컨버팅 할수있게 정의 해놓은 클래스를 사용 한다.
			jsonSerializerSettings = JsonConvert.DefaultSettings();
			jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;
			if (jsonConverters != null)
			{
				foreach (JsonConverter jsonConverter in jsonConverters)
				{
					jsonSerializerSettings.Converters.Add(jsonConverter);
				}
			}
		}
		#endregion 생성자

		#region 비공개 정적 변수
		/// <summary>
		/// string : TypeName
		/// object :데이타
		/// </summary>
		private static Dictionary<string, object> _data { get; } = new Dictionary<string, object>();
		/// <summary>
		/// string : TypeName
		/// bool :데이타 로드 여부
		/// </summary>
		/// <typeparam name=""></typeparam>
		private static Dictionary<string, bool> _isDataLoad { get; } = new Dictionary<string, bool>();
        /// <summary>
		/// JsonSeriallizerSetting
		/// </summary>
        private static JsonSerializerSettings jsonSerializerSettings;
		#endregion 비공개 정적 변수

		#region 공개 정적 변수
		/// <summary>
		/// 마지막에 저장하거나 로드한 경로
		/// </summary>
		public static string lastPath { get; private set; }
		#endregion 공개 정적 변수

		#region 공개 정적 함수
		/// <summary>
		/// 1. 데이터 사용시 해당 위치에서 캐시후 사용 하는게 좋음.
		/// 2. 불러 올때 해당 클래스 타입 이름으로 검색해서 가져옴
		/// forcing_load = 캐싱 되어있는 것 과 상관없이 파일을 불러와서 읽을 건지 여부
		/// file_name = 파일 이름을 넣을 경우 해당 이름의 파일로 로드, 없을 경우 클래스 이름으로 로드
		/// prefix = 파일 이름 접두
		/// suffix = 파일 이름 접미
		/// </summary>
		public static T Get<T>(bool forcing_load = false, string path = "") where T : new()
		{
			// 파일 경로 이걸로 데이타 컨테이너 키값으로 사용 한다.
			string fullPath = string.IsNullOrEmpty(path) ? $"{Application.streamingAssetsPath}/Data/Json/{typeof(T).Name}.json" : path;
			// 데이타 로드 여부 키값 검사
			if (!_isDataLoad.ContainsKey(fullPath))
			{
				_isDataLoad[fullPath] = false;
			}
			// 데이타 키값 검사
			if (!_data.ContainsKey(fullPath))
			{
				_data[fullPath] = new T();
			}
			// 데이타 로드한적 없으면 로드
			if (!_isDataLoad[fullPath] || forcing_load)
			{
				_data[fullPath] = Load<T>(fullPath);
				_isDataLoad[fullPath] = true;
			}
			return (T)_data[fullPath];
		}

		public static void Set<T>(T data, string path = "") where T : new()
		{
			string fullPath = string.IsNullOrEmpty(path) ? $"{Application.streamingAssetsPath}/Data/Json/{typeof(T).Name}.json" : path;
			_data[fullPath] = data;
			Save(data, fullPath);
		}

		/// <summary>
		/// path : 입력된 경로의 파일을 로드 한다.
		/// T : 해당 타입으로 컨버팅 해준다.
		/// </summary>
		public static T Load<T>(string path) where T : new()
		{
            T result = default(T);

			// 파일이 없으면
			if (!File.Exists(path))
			{
				// 폴더 경로 받아오기
				string directory_path = Path.GetDirectoryName(path);
				// 폴더 경로가 없으면
				if (!Directory.Exists(directory_path))                  
				{
					// 폴더 경로 생성
					Directory.CreateDirectory(directory_path);          
				}

				// 처음 데이타 파일 생성시 IOnBeforeJsonCreate 를 상속 받았는지 검사 후 상속 받았으면 OnBeforeCreate() 를 실행 해준다.
				T create = new T();
				if (typeof(T).GetInterfaces().Where(type => type.Equals(typeof(IOnBeforeJsonCreate))).Count() > 0)
				{
					(create as IOnBeforeJsonCreate).OnBeforeCreate();
				}

				// 파일 생성
				Save(create, path);
			}

			// json 파일에서 읽은 문자열 저장할 변수
			string json_string = string.Empty;
			// 파일 열어서 문자열 저장 후 닫음
			using (StreamReader file = new StreamReader(path))
			{
				json_string = file.ReadToEnd();
				file.Close();
			}

			// 읽은 문자열 구조체로 Deserialize 하고 리턴함
			try
			{
				result = JsonConvert.DeserializeObject<T>(json_string, jsonSerializerSettings);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}

			lastPath = path;

			return result;
		}

		public static void Save<T>(T data, string path) where T : new()
		{
			// 폴더 경로 받아오기
			string directory_path = Path.GetDirectoryName(path);
			// 폴더 경로가 없으면
			if (!Directory.Exists(directory_path))
			{
				// 폴더 경로 생성
				Directory.CreateDirectory(directory_path);
			}

			// 저장할 데이타 json 형태의 문자열로 변환
			string json_string = JsonConvert.SerializeObject(data, Formatting.Indented, jsonSerializerSettings);
			// 파일 생성후 열어서 문자열 저장 후 닫음
			using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
			{
				using (StreamWriter stream_writer = new StreamWriter(file, Encoding.UTF8))
				{
					stream_writer.WriteLine(json_string);
					stream_writer.Close();
				}
				file.Close();
			}

			lastPath = path;
		}
		#endregion 공개 정적 함수
	}
}

//#region 비공개 정적 함수
//private static string GetFilePath(RootPath root_path, string file_name)
//{
//	string rootPath = Application.streamingAssetsPath;
//	switch (root_path)
//	{
//		case RootPath.persistentData:
//			rootPath = Application.persistentDataPath;
//			break;
//		case RootPath.temporaryCache:
//			rootPath = Application.temporaryCachePath;
//			break;
//		case RootPath.StreamingAssets:
//		default:
//			break;
//	}
//	return $"{rootPath}/Data/Setting/{file_name}"; //string.Format(@"{0}/Data/Setting/{1}.json", rootPath, file_name);
//}
//#endregion 비공개 정적 함수

///// <summary>
///// Try to create the converter of specified type.
///// </summary>
///// <returns>The converter.</returns>
///// <param name="type">Type.</param>
//private static JsonConverter CreateConverter(Type type)
//{
//	try
//	{
//		return Activator.CreateInstance(type) as JsonConverter;
//	}
//	catch (Exception exception)
//	{
//		Debug.LogErrorFormat("Can't create JsonConverter {0}:\n{1}", type, exception);
//	}
//
//	return null;
//
//}
//
///// <summary>
///// Find all the valid converter types.
///// </summary>
///// <returns>The types.</returns>
//private static Type[] FindConverterTypes()
//{
//	Assembly.GetAssembly(typeof(JsonHelper)).GetTypes()
//		.Where(type => typeof(IGetConverter).IsAssignableFrom(type) && !typeof(IGetConverter).Equals(type))
//		.ToArray();
//	return AppDomain.CurrentDomain.GetAssemblies()
//		.SelectMany((assembly) => assembly.GetTypes())
//		.Where((type) => typeof(JsonConverter).IsAssignableFrom(type))
//		.Where((type) => !type.IsAbstract && !type.IsGenericTypeDefinition)
//		.Where((type) => null != type.GetConstructor(new Type[0]))
//		.Where((type) => null != type.Namespace && type.Namespace.StartsWith("Util_.JsonHelper_.Converter_"))
//		.ToArray();
//
//}
//
//public static T ConverUnityStruct<T>(string json)
//{
//	T convert = JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
//	return convert;
//}

///// <summary>
///// Json Unity ThiredParty 로드 여부
///// </summary>
//private static bool _initialize = false;
//private static void Initialize()
//{
//    if (_initialize)
//        return;
//
//    _initialize = true;
//
//    IEnumerable<JsonConverter> _customs = FindConverterTypes().Select((type) => CreateConverter(type));
//    IEnumerator<JsonConverter> jci = _customs.GetEnumerator();
//
//    setting = JsonConvert.DefaultSettings();
//
//    setting.DefaultValueHandling = DefaultValueHandling.Populate;
//
//    while (jci.MoveNext())
//    {
//        setting.Converters.Add(jci.Current);
//    }
//}
