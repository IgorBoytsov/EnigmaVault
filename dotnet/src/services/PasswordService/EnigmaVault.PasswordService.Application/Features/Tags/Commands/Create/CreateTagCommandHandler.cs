using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.Models;
using MediatR;
using Shared.Kernel.Exceptions;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Commands.Create
{
    internal sealed class CreateTagCommandHandler(
        ITagRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateTagCommand, Result<Unit>>
    {
        private ITagRepository _repository = repository;
        private IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tag = Tag.Create(request.UserId, request.Name);

                await _repository.AddAsync(tag, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (DomainException ex)
            {
                return ex.Error;
            }
            catch (Exception)
            {
                return Error.New(ErrorCode.Server, "Произошла ошибка на стороне сервера");
            }
        }
    }
}
