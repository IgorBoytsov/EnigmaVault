using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.DeleteCommon
{
    internal sealed class DeleteCommonIconCommandHandler(
        IUnitOfWork unitOfWork,
        IIconRepository repository) : IRequestHandler<DeleteCommonIconCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIconRepository _repository = repository;

        public async Task<Result<Unit>> Handle(DeleteCommonIconCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maybeIcon = await _repository.GetAsync(request.Id, token: cancellationToken);

                if (maybeIcon.HasValue)
                {
                    _repository.Remove(maybeIcon.Value);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }

                return Error.NotFound("Icon", request.Id);
            }
            catch (Exception)
            {
                return Error.New(ErrorCode.Server, "Произошла ошибка на стороне сервера");
            }
        }
    }
}