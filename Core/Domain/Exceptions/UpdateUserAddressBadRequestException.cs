
using Domain.Exceptions.BadRequests;

namespace Store.C44.G02.Domain.Exceptions
{
    public class UpdateUserAddressBadRequestException() : BadRequestException("Invalid operation while updating address")
    {
    }
}
