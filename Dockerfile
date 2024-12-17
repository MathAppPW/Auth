FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
WORKDIR /app

# Copy the solution file
COPY *.sln ./

# Copy project files
COPY Auth/*.csproj Auth/
COPY Auth.Dal/*.csproj Auth.Dal/
COPY Auth.Dal.Interfaces/*.csproj Auth.Dal.Interfaces/
COPY Auth.Models/*.csproj Auth.Models/

# Restore all projects via the solution
RUN dotnet restore Auth/Auth.csproj

# Now copy everything (including source code)
COPY . .

# Expose the port your application listens on
EXPOSE 5080

# Run the Auth project (adjust if your main startup project is different)
CMD ["dotnet", "run", "--project", "Auth", "--urls=http://0.0.0.0:5080"]

