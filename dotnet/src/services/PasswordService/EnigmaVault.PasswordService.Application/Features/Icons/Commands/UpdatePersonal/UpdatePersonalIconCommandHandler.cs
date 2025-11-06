using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.ValueObjects.Password;
using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIcon;
using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIconCategory;
using MediatR;
using Shared.Kernel.Exceptions;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.UpdatePersonal
{
    internal sealed class UpdatePersonalIconCommandHandler(IUnitOfWork unitOfWork, IIconRepository repository) : IRequestHandler<UpdatePersonalIconCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIconRepository _repository = repository;

        public async Task<Result<Unit>> Handle(UpdatePersonalIconCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maybeCategory = await _repository.GetAsync(request.Id, request.UserId, cancellationToken);

                if (maybeCategory.HasValue)
                {
                    maybeCategory.Value.UpdateName(IconName.Create(request.Name));
                    maybeCategory.Value.UpdateCategory(IconCategoryId.Create(request.IconCategoryId));
                    maybeCategory.Value.UpdateSvg(SvgCode.Create(request.SvgCode));
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