using AutoMapper;
using MediatR;
using RentACar.Application.Services.Repositories;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Features.Brands.Commands.Delete;

public class DeletedBrandCommand : IRequest<DeletedBrandResponse>
{
    public Guid Id { get; set; }


    public class DeletedBrandCommandHandler : IRequestHandler<DeletedBrandCommand, DeletedBrandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;

        public DeletedBrandCommandHandler (IMapper mapper, IBrandRepository brandRepository)
        {
            _mapper = mapper;
            _brandRepository = brandRepository;
        }

        public async Task<DeletedBrandResponse> Handle (DeletedBrandCommand request, CancellationToken cancellationToken)
        {
            Brand? brand = await _brandRepository.GetAsync(b => b.Id == request.Id);

            brand = _mapper.Map(request, brand);

            await _brandRepository.DeleteAsync(brand);

            DeletedBrandResponse? deletedBrandCommand = _mapper.Map<DeletedBrandResponse>(brand);

            return deletedBrandCommand;
        }
    }
}