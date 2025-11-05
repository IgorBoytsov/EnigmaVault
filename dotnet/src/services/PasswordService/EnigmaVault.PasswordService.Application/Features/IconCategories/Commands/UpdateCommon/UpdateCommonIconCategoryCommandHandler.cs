using Common.Core.Primitives;
using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIconCategory;
using MediatR;
using Shared.Kernel.Exceptions;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.UpdateCommon
{
    internal sealed class UpdateCommonIconCategoryCommandHandler(IUnitOfWork unitOfWork, IIconCategoryRepository repository) : IRequestHandler<UpdateCommonIconCategoryCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIconCategoryRepository _repository = repository;

        public async Task<Result<Unit>> Handle(UpdateCommonIconCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maybeCategory = await _repository.GetAsync(request.Id, token: cancellationToken);

                if (maybeCategory.HasValue)
                {
                    maybeCategory.Value.UpdateName(IconCategoryName.Create(request.Name));
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }

                return Error.NotFound(request.Name, request.Id);
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