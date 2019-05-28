using Orleans.Services;

namespace Orleans.NewTestKit
{
    internal interface ITestKitServiceClient : IGrainServiceClient<ITestKitService>, ITestKitService
    {
    }
}
