using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


namespace Jin.Utility
{
    public class FileManager
    {

        static public string GetStreamingAssetsPath(string fileName)
        {
            string path = Application.streamingAssetsPath + Path.DirectorySeparatorChar + "AssetBundle" + Path.DirectorySeparatorChar + fileName;
            return path;
        }

        static public string GetBundleDataPath(string fileName)
        {
            string path = Application.persistentDataPath + Path.DirectorySeparatorChar + fileName;
            return path;
        }


        static public bool Save(string path, byte[] bytes)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fileStream);

                bw.Write(bytes);

                bw.Close();
                fileStream.Close();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                Debug.LogError("XFileManager.Save By Path - UnauthorizedAccessException " + path);
                return false;
            }
        }

        static public bool Save(string path, string str)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fileStream);

                sw.Write(str);

                sw.Close();
                fileStream.Close();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                Debug.LogError("XFileManager.Save - UnauthorizedAccessException " + path);
                return false;
            }
        }

        static public FileStream Load(string path)
        {
            if (File.Exists(path) == false)
                return null;

            //FileStream fileStream = File.Open( path, FileMode.Open, FileAccess.Read );
            //FileStream fileStream = File.Open( path, FileMode.Open, FileAccess.Read, FileShare.None );
            FileStream fileStream = File.Open(path, FileMode.Open);
            return fileStream;
        }

        static public byte[] LoadByte(string path)
        {
            FileStream fileStream = Load(path);
            if (fileStream == null)
                return null;

            BinaryReader br = new BinaryReader(fileStream);
            byte[] buffer = br.ReadBytes((int)fileStream.Length);
            br.Close();

            return buffer;
        }

        static public string LoadText(string path)
        {
            if (File.Exists(path) == false)
                return null;

            StreamReader reader = new StreamReader(path);
            string content = reader.ReadToEnd();
            reader.Close();

            return content;
        }

        static public bool IsExists(string path)
        {
            return File.Exists(path);
        }

        static public void Copy(string src, string dest, bool overwrite)
        {
            File.Copy(src, dest, overwrite);
        }

        static public void Delete(string path)
        {
            File.Delete(path);
        }

        static public bool Rename(string src, string dest, bool overwrite)
        {
            if (IsExists(dest) == true)
            {
                if (overwrite == false)
                {
                    Debug.LogWarning("XFileManager Rename is Failed. " + dest);
                    return false;
                }

                Delete(dest);
            }

            File.Move(src, dest);
            return true;
        }

        static public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        static public string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        static public string GetDirectoryName(string path)
        {
            // from = 	Assets/_Hero/fungi_breath/fungi_breath@skill01.FBX
            // to = 	Assets/_Hero/fungi_breath
            return Path.GetDirectoryName(path);

        }

        static public void CreateDirectory(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }

        static public bool IsExistDirectory(string path)
        {
            return Directory.Exists(path);
        }

        static public bool IsExistDirectory(string path, bool is_create)
        {
            if (Directory.Exists(path) == true)
                return true;

            if (is_create == true)
            {
                CreateDirectory(path);
                return true;
            }
            else
                return false;
        }
        
        

    }
}