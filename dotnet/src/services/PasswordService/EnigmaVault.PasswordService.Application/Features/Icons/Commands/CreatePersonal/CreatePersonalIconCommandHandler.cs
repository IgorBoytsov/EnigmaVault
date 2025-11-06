using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.Models;
using MediatR;
using Shared.Kernel.Exceptions;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.CreatePersonal
{
    internal sealed class CreatePersonalIconCommandHandler(
        IUnitOfWork unitOfWork,
        IIconRepository repository) : IRequestHandler<CreatePersonalIconCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIconRepository _repository = repository;

        public async Task<Result<Unit>> Handle(CreatePersonalIconCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var icon = Icon.Create(request.Name, request.UserId, request.SvgCode, request.IconCategoryId);

                await _repository.AddAsync(icon, cancellationToken);
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