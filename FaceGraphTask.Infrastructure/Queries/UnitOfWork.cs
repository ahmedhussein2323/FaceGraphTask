using FaceGraphTask.Core.Entities;

namespace FaceGraphTask.Infrastructure.Queries
{
    public class UnitOfWork 
    {
        private Repository<Image> _imageRepository;
        private Repository<User> _userRepository;

        public Repository<User> UserRepository => _userRepository ?? (_userRepository = new Repository<User>("User"));
        public Repository<Image> ImageRepository => _imageRepository ?? (_imageRepository = new Repository<Image>("Image"));
    }
}
