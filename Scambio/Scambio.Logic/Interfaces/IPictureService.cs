using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scambio.Domain.Models;

namespace Scambio.Logic.Interfaces
{
    public interface IPictureService
    {
        void AddPicture(Picture picture);
        void CreatePicture(string filename, string path, Stream inputStream);       
        string GeneratePictureFilename(Picture picture, string postfix = "");
        string GeneratePictureFilename(Guid pictureId, string pictureSecret, string extension, string postfix = "");
        Picture GetPicture(Guid id);
        string GetPictureLocation(string pictureStorage, Guid creator, Guid pictureId, string postfix = "");
        string GetPictureLocation(string pictureStorage, Guid creator, Picture picture, string postfix = "");

    }
}
