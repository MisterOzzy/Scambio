using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scambio.DataAccess.Infrastructure;
using Scambio.Domain.Models;
using Scambio.Logic.Interfaces;

namespace Scambio.Logic
{
    public class PictureService : IPictureService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PictureService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddPicture(Picture picture)
        {
            _unitOfWork.PictureRepository.Add(picture);
            _unitOfWork.Save();
        }

        public void CreatePicture(string filename, string path, Stream inputStream)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fullPath = Path.Combine(path, filename);

            FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);

            inputStream.CopyTo(fileStream);
            inputStream.Close();
            fileStream.Close();

        }

      
        public string GeneratePictureFilename(Guid pictureId, string pictureSecret, string extension)
        {
            return $"{pictureId}_{pictureSecret}.{extension}";
        }

        public string GeneratePictureFilename(Picture picture)
        {
            return GeneratePictureFilename(picture.Id, picture.Secret, picture.Extension);
        }

        public Picture GetPicture(Guid id)
        {
            return _unitOfWork.PictureRepository.GetById(id);
        }

        public string GetPictureLocation(string pictureStorage, Guid creator, Guid pictureId)
        {
            return GetPictureLocation(pictureStorage, creator, GetPicture(pictureId));
        }

        public string GetPictureLocation(string pictureStorage, Guid creator, Picture picture)
        {
            return Path.Combine(pictureStorage, creator.ToString(),
                GeneratePictureFilename(picture.Id, picture.Secret, picture.Extension));
        }
    }
}
