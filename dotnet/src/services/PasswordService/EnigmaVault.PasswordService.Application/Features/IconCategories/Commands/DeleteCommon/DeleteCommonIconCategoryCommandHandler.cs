using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.DeleteCommon
{
    internal sealed class DeleteCommonIconCategoryCommandHandler(IUnitOfWork unitOfWork, IIconCategoryRepository repository) : IRequestHandler<DeleteCommonIconCategoryCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIconCategoryRepository _repository = repository;

        public async Task<Result<Unit>> Handle(DeleteCommonIconCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maybeCategory = await _repository.GetAsync(request.Id, token: cancellationToken);

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