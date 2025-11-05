using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.DeletePersonal
{
    internal sealed class DeletePersonalIconCategoryCommandHandler(IUnitOfWork unitOfWork, IIconCategoryRepository repository) : IRequestHandler<DeletePersonalIconCategoryCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIconCategoryRepository _repository = repository;

        public async Task<Result<Unit>> Handle(DeletePersonalIconCategoryCommand request, CancellationToken cancellationToken)
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