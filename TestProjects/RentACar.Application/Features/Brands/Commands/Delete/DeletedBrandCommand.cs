using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Caching;
using MenCore.Application.Pipelines.Transaction;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.Brands.Commands.Delete;

public class DeletedBrandCommand : IRequest<DeletedBrandResponse>, ICacheRemoverRequest, ITransactionalRequest
{
    public Guid Id { get; set; }

    public string? CacheGroupKey => "GetBrands";

    public string CacheKey => string.Empty;

    public bool BypassCache => false;

    public class DeletedBrandCommandHandler : IRequestHandler<DeletedBrandCommand, DeletedBrandResponse>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public DeletedBrandCommandHandler(IMapper mapper, IBrandRepository brandRepository)
        {
            _mapper = mapper;
            _brandRepository = brandRepository;
        }

        public string? CacheKey => string.Empty;

        public bool BypassCache => false;

        public string? CacheGroupKey => "GetBrands";

        public async Task<DeletedBrandResponse> Handle(DeletedBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetAsync(b => b.Id == request.Id);

            brand = _mapper.Map(request, brand);

            await _brandRepository.DeleteAsync(brand);

            var deletedBrandCommand = _mapper.Map<DeletedBrandResponse>(brand);

            return deletedBrandCommand;
        }
    }
}