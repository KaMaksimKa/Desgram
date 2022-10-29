using AutoMapper;
using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class PublicationService:IPublicationService
    {
        private readonly ApplicationContext _context;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public PublicationService(ApplicationContext context,IFileService fileService,IMapper mapper)
        {
            _context = context;
            _fileService = fileService;
            _mapper = mapper;
        }
        public async Task CreatePublicationAsync(CreatePublicationModel model, User user)
        {
            var publication = new Publication()
            {
                Id = Guid.NewGuid(),
                User = user,
                Description = model.Description,
                AmountLikes = 0,
                Comments = new List<Comment>(),
                LikesPublication = new List<LikePublication>(),
                ImagesPublication = model.Images.Select(file =>
                    new ImagePublication()
                    {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTimeOffset.Now.UtcDateTime,
                        User = user,
                        Path = _fileService.SaveImage(file)
                    }).ToList()
            };
            await _context.Publications.AddAsync(publication);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PublicationModel>> GetAllPublicationsAsync()
        {
            var publications = (await _context.Publications.Include(p=>p.ImagesPublication).ToListAsync())
                .Select(p=> _mapper.Map<PublicationModel>(p)).ToList();
            return publications;
        }

        public async Task AddComment(CreateCommentModel model, Guid publicationId, User user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteComment(Guid commentId, User user)
        {
            throw new NotImplementedException();
        }

        public async Task AddLike(Guid publicationId, User user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteLike(Guid publicationId, User user)
        {
            throw new NotImplementedException();
        }

        public async Task AddLikeComment(Guid commentId, User user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteLikeComment(Guid commentId, User user)
        {
            throw new NotImplementedException();
        }


        public async Task GetComments(Guid publicationId)
        {
            throw new NotImplementedException();
        }
    }
}
