using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.DeletePersonal
{
    internal sealed class DeletePersonalIconCommandHandler(IUnitOfWork unitOfWork, IIconRepository repository) : IRequestHandler<DeletePersonalIconCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIconRepository _repository = repository;

        public async Task<Result<Unit>> Handle(DeletePersonalIconCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maybeCategory = await _repository.GetAsync(request.Id, request.UserId, cancellationToken);

                if (maybeCategory.HasValue)
                {
                    _repository.Remove(maybeCategory.Value);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }

                return Error.NotFound("IconCategory", request.Id);
            }
            catch (Exception)
            {
                return Error.New(ErrorCode.Server, "Произошла ошибка на стороне сервера");
            }
        }
    }
}