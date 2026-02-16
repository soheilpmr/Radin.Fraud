var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Radin_Fraud_Identity>("radin-fraud-identity");

builder.Build().Run();
