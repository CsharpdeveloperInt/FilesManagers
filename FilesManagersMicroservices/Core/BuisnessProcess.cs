using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Net.Mail;
using FilesManagersDbContext.Models;
using Ionic.Zip;


namespace FilesManagersMicroservices.Core
{
    public class BuisnessProcess
    {
        public string GlobalPath = WebConfigurationManager.AppSettings["GlobalPath"];

        protected FilesManagersMicroservicesEntities FilesManagers { get; set; }

        public BuisnessProcess()
        {
            FilesManagers = new FilesManagersMicroservicesEntities();
        }

        /// <summary>
        /// Метод для создания директории (в глобальной папке)
        /// </summary>
        /// <param name="name">Имя новой директории</param>
        /// <returns>Возвращает True если директория создана</returns>
        public bool CreateDirectory(string name)
        {
            try
            {
                var dirInfo = new DirectoryInfo(GlobalPath);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                dirInfo.CreateSubdirectory(name);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Метод для удаления директории (в глобальной папке)
        /// </summary>
        /// <param name="name">Имя удаляемой директории</param>
        /// <returns>Возвращает True если директория удалена</returns>
        public bool DeleteDirectory(string name)
        {
            try
            {
                var deleteDir = Path.Combine(GlobalPath, name);
                var dirInfo = new DirectoryInfo(deleteDir);
                dirInfo.Delete(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Методя дл переименования директории (в глобальной папке)
        /// </summary>
        /// <param name="oldName">Старое имя каталога</param>
        /// <param name="newName">Новое имя каталога</param>
        /// <returns>Возвращает True если директория переименована</returns>
        public bool RenameDirectory(string oldName, string newName)
        {
            try
            {
                var renameDir = Path.Combine(GlobalPath, oldName);
                var newrenameDir = Path.Combine(GlobalPath, newName);
                Directory.Move(renameDir, newrenameDir);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Метод для загрузки файла в определеннную директорию (если директорию не существует закачивает в корневую папку)
        /// </summary>
        /// <param name="file">Закачиваемый файл</param>
        /// <param name="dirname">Директория в которую требуется закачать файл</param>
        /// <returns>Возвращает True если файл загружен</returns>
        public bool UploadFileInDirectory(HttpPostedFileBase file, string dirname)
        {
            try
            {
                var dirfind = new DirectoryInfo(Path.Combine(GlobalPath, dirname));
                var dirInfo = new DirectoryInfo(GlobalPath);
                var fileName = Path.GetFileName(file.FileName);
                if (!dirfind.Exists)
                {
                    file.SaveAs(dirInfo +"//" +fileName);
                }
                else
                {
                    file.SaveAs(dirfind + "//" + fileName);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }



        /// <summary>
        /// Метод для удаления файла
        /// </summary>
        /// <param name="namefile">Имя удаляемого файла</param>
        /// <returns>Возвращает True если файл удален</returns>
        public bool DeleteFile(string namefile)
        {
            try
            {
                var deleteDir = Path.Combine(GlobalPath, namefile);
                var fileInf = new FileInfo(deleteDir);
                if (!fileInf.Exists) return false;
                fileInf.Delete();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Метод для переименования файла
        /// </summary>
        /// <param name="oldnameFile">Старое название файла</param>
        /// <param name="newnameFile">Новое название файла</param>
        /// <returns>Возращает True если файл переименован</returns>
        public bool RenameFile(string oldnameFile, string newnameFile)
        {
            try
            {
                var newFileInfo = new FileInfo(GlobalPath+"//"+oldnameFile);
                newFileInfo.MoveTo(GlobalPath + "//" + newnameFile);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Метод для скачивания файла
        /// </summary>
        /// <param name="filename">Имя файла</param>
        /// <returns>Файл</returns>
        public FileResult GetFile(string filename)
        {
            var filePath = GlobalPath + "//" + filename;
            var contentType = GetContentType(filename);
            return new FilePathResult(filePath, contentType);     
        }



        /// <summary>
        /// Метод для получения типа контента в файле (вспомогательная функция)
        /// </summary>
        /// <param name="fileName">Имя файл с раширением</param>
        /// <returns>Тип Контента(строка)</returns>
        public string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var contentType = "application/unknown";

            if (extension == null) return contentType;
            var reg = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(extension.ToLower());
            var registryContentType = reg?.GetValue("Content Type") as string;
            if (string.IsNullOrWhiteSpace(registryContentType)) return contentType;

            contentType = reg.GetValue("Content Type") as string;
            return contentType;
        }



        /// <summary>
        /// Метод получения папки с файлами в виде ZIP
        /// </summary>
        /// <param name="folderName">Имя папки</param>
        /// <returns>Файл ZIP</returns>
        public FileResult GetZipFolder(string folderName)
        {
            var downloadFileName = $"YourDownload-{DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss")}.zip";
            using (var zip = new ZipFile())
            {
                zip.AddDirectory(GlobalPath+"\\"+folderName);
                var output = new MemoryStream();
                zip.Save(output);
                output.Position = 0;
                var result = new FileStreamResult(output, "application/zip") { FileDownloadName = downloadFileName };
                return result;
            }
        }



        /// <summary>
        /// Метод для пакетного удаления директорий (в глобальной папке)
        /// </summary>
        /// <param name="name">Массив удаляемых директорий</param>
        /// <returns>Возвращает True если директории удалены</returns>
        public bool DeleteMassDirectory(string[] name)
        {
            try
            {
                foreach (var dirInfo in name.Select(item => Path.Combine(GlobalPath, item)).Select(deleteDir => new DirectoryInfo(deleteDir)))
                {
                    dirInfo.Delete(true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Метод для массового удаления файлов
        /// </summary>
        /// <param name="namefile">Массив удаляемых файлов</param>
        /// <returns>Возвращает True если файлы удалены</returns>
        public bool DeleteMassFile(string[] namefile)
        {
            try
            {
                foreach (var fileInf in namefile.Select(item => Path.Combine(GlobalPath, item)).Select(deleteDir => new FileInfo(deleteDir)).Where(fileInf => fileInf.Exists))
                {
                    fileInf.Delete();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Метод для отправки данных  по почте
        /// </summary>
        /// <param name="files">Массив имен файлов для отправки</param>
        /// <param name="email">Email на который надо отправить</param>
        /// <returns>Возвращает True если файлы отправлены</returns>
        public bool SendEmail(string[] files,string email)
        {    
            var from = new MailAddress("csharpdeveloper@list.ru", "FilesMicroServices");
            var to = new MailAddress(email);
            var mail = new MailMessage(from, to)
            {
                Subject = "Вам отправлены файлы",
                Body = "Смотрите вложенные файлы"
            };
            foreach (var item in files.Select(item => Path.Combine(GlobalPath, item)).Select(deleteDir => new FileInfo(deleteDir)).Where(fileInf => fileInf.Exists))
            {
                mail.Attachments.Add(new Attachment(item.ToString()));
            }
        
            var smtp = new SmtpClient("smtp.mail.ru", 25)
            {
                Credentials = new NetworkCredential("csharpdeveloper@list.ru", "***@"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            try
            {
                smtp.Send(mail);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        /// <summary>
        /// Метод для сохранения файла в БД
        /// </summary>
        /// <param name="filename">Объект таблицы Files В БД</param>
        /// <param name="file">Объект пришедшего файла</param>
        /// <returns>Возвращает True если файлы сохранены</returns>
        public bool SaveFileInDb(string filename, HttpPostedFileBase file)
        {
            try
            {
                byte[] imageData;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    imageData = binaryReader.ReadBytes(file.ContentLength);
                }
                // установка массива байтов
                var f = new Files
                {
                    binaryFile = imageData,
                    namePath = filename
                };
                //Загружаем в БД и сохраняем
                FilesManagers.Files.Add(f);
                FilesManagers.SaveChanges();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Метод для получения ссылки на файл.
        /// </summary>
        /// <param name="filename">Имя файла</param>
        /// <returns>Строку для скачки файла</returns>
        public string GetUrlForFile(string filename)
        {
            try
            {
                var url = Path.Combine(GlobalPath, filename);
                return url;
            }
            catch (Exception)
            {
                return "none";
            }
        }



        /// <summary>
        /// Метод для получения информации по диску корневого пути
        /// </summary>
        /// <returns>Строку сколько свободно на диске места</returns>
        public string GetSizeGlobalPath()
        {
            try
            {
                var drives = DriveInfo.GetDrives();
                var globalRut = Path.GetPathRoot(GlobalPath);
                foreach (var disk in drives.Where(disk => globalRut != null && disk.Name.Contains(globalRut)))
                {
                    return (((disk.AvailableFreeSpace/1024)/1024/1024) + " гб");
                }
                return "Неизвестно";
            }
            catch (Exception e)
            {
                return "none "+" Error " + e;
            }
        }
    }
}
