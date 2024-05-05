using Microsoft.AspNetCore.Identity;

namespace ProjectDelivery.Classes
{
    public class OperationResult
    {
        public bool Success { get; }
        public string ErrorMessage { get; }
        public IEnumerable<IdentityError> Errors { get; set; }
        public OperationResult(bool success, string errorMessage = "")
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }
}
