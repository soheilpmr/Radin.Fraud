	
	-- 1. Create the Linked Server
	EXEC sp_addlinkedserver   
	   @server = N'10.81.15.128',       -- The name you will use to query the server
	   @srvproduct = N'SQL Server';     -- Specifies that it is a standard SQL Server
	GO

	-- 2. Configure the Login Credentials
	-- This tells your local server how to authenticate with the remote server
	EXEC sp_addlinkedsrvlogin   
	   @rmtsrvname = N'10.81.15.128',   
	   @useself = N'False',             -- False means we are providing specific credentials
	   @locallogin = NULL,              -- Applies to all local users (can restrict if needed)
	   @rmtuser = N'sa',-- The SQL Login for 10.81.15.128
	   @rmtpassword = N'123qwe!@#QWE';
	GO

	-- 3. Enable RPC (Remote Procedure Calls) - Optional but recommended for executing remote commands
	EXEC sp_serveroption @server=N'10.81.15.128', @optname=N'rpc', @optvalue=N'true';
	EXEC sp_serveroption @server=N'10.81.15.128', @optname=N'rpc out', @optvalue=N'true';
	GO