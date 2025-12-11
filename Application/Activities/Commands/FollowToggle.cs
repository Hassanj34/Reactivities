using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands
{
    public class FollowToggle
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required string TargetUserId { get; set; }
        }

        public class Handler(AppDbContext dbContext, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var observer = await userAccessor.GetUserAsync();
                var target = await dbContext.Users.FindAsync([request.TargetUserId], cancellationToken);

                if (target == null) return Result<Unit>.Failure("Targer user not found", 400);

                var following = await dbContext.UserFollowings.FindAsync([observer.Id, target.Id], cancellationToken);

                if (following == null)
                {
                    dbContext.UserFollowings.Add(new UserFollowing
                    {
                        ObserverId = observer.Id,
                        TargetId = target.Id
                    });
                }
                else dbContext.UserFollowings.Remove(following);

                var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;

                return result
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Error updating following", 400);
            }
        }
    }
}