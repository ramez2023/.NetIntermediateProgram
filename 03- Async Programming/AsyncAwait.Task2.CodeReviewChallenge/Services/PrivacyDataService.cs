using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Services;

public class PrivacyDataService : IPrivacyDataService
{
    /*
    
    Old Code:
    public Task<string> GetPrivacyDataAsync()
    {
        return new ValueTask<string>("This Policy describes how async/await processes your personal data," +
                                     "but it may not address all possible data processing scenarios.").AsTask();
    }

    Not Need to return task here. 
    ValueTask is a lightweight alternative to Task that is useful for avoiding unnecessary heap allocations in performance-critical code. 
    The async and await pattern can be used with ValueTask as well.

    */



    public ValueTask<string> GetPrivacyDataAsync()
    {
        return new ValueTask<string>("This Policy describes how async/await processes your personal data," +
                                     "but it may not address all possible data processing scenarios.");
    }
}
