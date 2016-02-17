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
        void CreatePicture(string filename, string path, Stream inputStream);
        string GeneratePictureFilename(Picture picture);
        string GeneratePictureFilename(Guid pictureId, string pictureSecret, string extension);
        Picture GetPicture(Guid id);
        string GetPictureLocation(string pictureStorage, Guid creator, Guid pictureId);
        string GetPictureLocation(string pictureStorage, Guid creator, Picture picture);

    }
}
