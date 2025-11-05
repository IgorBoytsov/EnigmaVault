using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.Models;
using MediatR;
using Shared.Kernel.Exceptions;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.CreateCommon
{
    internal sealed class CreateCommonIconCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IIconCategoryRepository repository) : IRequestHandler<CreateCommonIconCategoryCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIconCategoryRepository _repository = repository;

        public async Task<Result<Unit>> Handle(CreateCommonIconCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = IconCategory.Create(request.Name, userId: null);

                await _repository.AddAsync(category, cancellationToken);
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