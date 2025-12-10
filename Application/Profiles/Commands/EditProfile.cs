using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Profiles.Commands
{
    public class EditProfile
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required string DisplayName { get; set; }
            public string Bio { get; set; } = string.Empty;
        }

        public class Handler(IUserAccessor userAccessor, AppDbContext dbContext) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await userAccessor.GetUserAsync();

                user.DisplayName = request.DisplayName;
                user.Bio = request.Bio;

                var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;

                return result
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Error updating profile", 400);
            }
        }
    }
}