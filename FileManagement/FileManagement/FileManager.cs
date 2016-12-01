using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
namespace FileManagement
{
    public class FileManager
    {
        #region PROGRAM'S RESOURCES' FILE'S ADMINISTRATION

        private string SourcePath;

        public FileManager(bool ResourceInitialize)
        {
            if(ResourceInitialize)
                Resource_Initialize();
        }

        public FileManager(string ResourcePath)
        {
            Resource_Initialize(ResourcePath);
        }   

        /// <summary>
        /// Inizializza cartella e file dentro alla cartella del progetto
        /// </summary>
        /// <returns></returns>
        public bool Resource_Initialize()
        {
            //inserisce dentro SourcePath l' indirizzo della cartella risorse del programma o la crea se non esiste
            try
            {
                string SourcePath = AppDomain.CurrentDomain.BaseDirectory;  //cartella del programma

                for (int slash = 0; slash < 3;)
                {
                    if (SourcePath.Substring(SourcePath.Length - 1, 1) == @"\")
                        slash++;

                    SourcePath = SourcePath.Substring(0, SourcePath.Length - 1);
                }

                SourcePath += @"\Resource";
                if (!Directory.Exists(SourcePath))
                    Directory.CreateDirectory(SourcePath);

                SourcePath += @"\Source.txt";
                if (!File.Exists(SourcePath))
                    File.Create(SourcePath).Close();

                this.SourcePath = SourcePath;
                return true;
            }
            catch
            { return false; }
        }

        public bool Resource_Initialize(string ResourceFolderPath)
        {
            //inserisce dentro SourcePath l' indirizzo della cartella risorse del programma o la crea se non esiste
            try
            {
                ResourceFolderPath += @"\Resource";
                if (!Directory.Exists(ResourceFolderPath))
                    Directory.CreateDirectory(ResourceFolderPath);

                ResourceFolderPath += @"\Source.txt";
                if (!File.Exists(ResourceFolderPath))
                    File.Create(ResourceFolderPath).Close();

                SourcePath = ResourceFolderPath;
                return true;
            }
            catch
            { return false; }
        }

        public bool Resource_Update(string[] content)
        {
            if (TryToWrite(SourcePath, content, false))
                return true;
            else
                return false;
        }

        public bool Resource_TryToRead(out string content)
        {
            content = "";
            if (TryToRead(SourcePath, out content))
                return true;
            else
                return false;
        }

        public bool Resource_TryToReadLines(out string[] content)
        {
            content = new string[0];
            if (TryToReadLines(SourcePath, out content))
                return true;
            else
                return false;
        }

        #endregion

        #region STATIC FUNCTIONS

        #region BROWSE DIALOG

        public static bool Browse_Folder(string Title, out string folderPath)
        {
            folderPath = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                Description = Title,
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = true,
            };
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                folderPath = fbd.SelectedPath;
                return true;
            }
            return false;
        }

        public static bool Browse_File(string Title, out string filePath, string filter = "All Files|*;")
        {
            filePath = "";
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                Filter = filter,
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                return true;
            }
            return false;
        }

        public static bool Browse_File(string Title, out string[] filePaths, string filter = "All Files|*;")
        {
            filePaths = new string[0];
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                Filter = filter,
                Multiselect = true,
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePaths = ofd.FileNames;
                return true;
            }
            return false;
        }

        public static bool Browse_File(string Title, out string filePath, string initialDir,string filter = "All Files|*;")
        {
            filePath = "";
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = initialDir,
                Filter = filter,
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                return true;
            }
            return false;
        }

        public static bool Browse_File(string Title, out string[] filePaths, string initialDir, string filter = "All Files|*;")
        {
            filePaths = new string[0];
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = initialDir,
                Filter = filter,
                Multiselect = true,
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePaths = ofd.FileNames;
                return true;
            }
            return false;
        }

        #endregion

        #region INPUT/OUTPUT FROM FILE

        public static string GetExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        public static bool HasExtension(string fileName, out string extension)
        {
            extension = Path.GetExtension(fileName);
            if (extension != null && extension != "")
                return true;
            else
                return false;
        }

        public static string Read(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch
            {
                return null;
            }
        }

        public static bool TryToRead(string filePath, out string content)
        {
            content = "";
            try
            {
                content = File.ReadAllText(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string[] ReadLines(string filePath)
        {
            string[] content;
            try
            {
                content = File.ReadAllLines(filePath);
            }
            catch
            {
                content = new string[0];
            }
            return content;
        }

        public static bool TryToReadLines(string filePath, out string[] content)
        {
            content = new string[0];
            try
            {
                content = File.ReadAllLines(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryToWrite(string filePath, string content, bool appendContent)
        {
            StreamWriter sw;
            try
            {
                sw = new StreamWriter(filePath, appendContent);
                sw.WriteLine(content);
                sw.Close();
                return true;
            }
            catch { return false; }
        }

        public static bool TryToWrite(string filePath, string[] content, bool appendContent)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    if (!appendContent)
                        File.WriteAllText(filePath, "");

                    File.AppendAllLines(filePath, content);
                    return true;
                }
            }
            catch { }
            return false;
        }

        #endregion

        #region MANAGEMENT

        public static bool CreateNewFile(string dirPath, string fileName, string extension = null, string content = null, bool evenIfExist = false)
        {
            //sistema l'estensione 
            if (extension == null)  //se l'utente non inserisce l'estensione
            {
                if (!HasExtension(fileName, out extension)) //controlla se il nome del file non ha un'estensione, se ce l'ha la mette in estension
                    return false;
            }
            else
                if (extension[0] != '.')   //se è inserita ma non è espressa correttamente (.*) la corregge
                extension = "." + extension;

            //crea il file
            if (evenIfExist)    //se deve creare un file del tipo: nome(n).estensione nel caso in cui esista già un file con lo stesso nome
            {
                string path = dirPath + @"\" + fileName + extension;
                if (File.Exists(path))  //se esiste un file con lo stesso nome
                    for (int n = 1; File.Exists(path); n++)
                        path = dirPath + @"\" + fileName + "(" + n + ")" + extension;

                File.WriteAllText(path, content);
                return true;
            }
            else //se non lo deve creare nel caso ne esista uno con lo stesso nome
            {
                if (!File.Exists(dirPath + @"\" + fileName + extension))//se il file non esiste viene creato e riempiti
                {
                    File.WriteAllText(dirPath + @"\" + fileName + extension, content);
                    return true;
                }
                return false;
            }
        }

        public static bool CreateNewFile(out string filePath, string dirPath, string fileName, string extension = null, string content = null, bool evenIfExist = false)
        {
            filePath = dirPath;
            //sistema l'estensione 
            if (extension == null)  //se l'utente non inserisce l'estensione
            {
                if (!HasExtension(fileName, out extension)) //controlla se il nome del file non ha un'estensione, se ce l'ha la mette in estension
                    return false;
            }
            else
                if (extension[0] != '.')   //se è inserita ma non è espressa correttamente (.*) la corregge
                extension = "." + extension;

            //crea il file
            if (evenIfExist)    //se deve creare un file del tipo: nome(n).estensione nel caso in cui esista già un file con lo stesso nome
            {
                filePath = dirPath + @"\" + fileName + extension;
                if (File.Exists(filePath))  //se esiste un file con lo stesso nome
                    for (int n = 1; File.Exists(filePath); n++)
                        filePath = dirPath + @"\" + fileName + "(" + n + ")" + extension;

                File.WriteAllText(filePath, content);
                return true;
            }
            else //se non lo deve creare nel caso ne esista uno con lo stesso nome
            {
                filePath = dirPath + @"\" + fileName + extension;
                if (!File.Exists(filePath))//se il file non esiste viene creato e riempiti
                {
                    File.WriteAllText(filePath, content);
                    return true;
                }
                return false;
            }
        }

        public static bool CreateDirectory(string dirPath, string newDirName, bool evenIfExist = false)
        {
            //crea la cartella 
            if (evenIfExist)    //se deve creare una cartella del tipo: nome(n) nel caso in cui esista già una cartella con lo stesso nome
            {
                string path = dirPath + @"\" + newDirName;
                if (Directory.Exists(path))  //se esiste una cartella con lo stesso nome
                    for (int n = 1; Directory.Exists(path); n++)
                        path = dirPath + @"\" + newDirName + "(" + n + ")";

                Directory.CreateDirectory(path);
                return true;
            }
            else //se non la deve creare nel caso ne esista una con lo stesso nome
            {
                //se la cartella non esiste viene creata
                Directory.CreateDirectory(dirPath + @"\" + newDirName);
                return true;
            }
        }

        public static bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        public static bool DeleteFile(string[] path)
        {
            for (int i = 0; i < path.Length; i++)
                if (File.Exists(path[i]))
                {
                    File.Delete(path[i]);
                    return true;
                }
            return false;
        }

        public static string[] WhatsInto(string dirPath)
        {
            return Directory.GetFiles(dirPath);
        }

        #endregion

        #endregion

    }
}
