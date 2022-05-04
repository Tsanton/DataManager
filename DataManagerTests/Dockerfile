FROM mcr.microsoft.com/dotnet/sdk:6.0

COPY ./DataManagerTests/DataManagerTests.csproj ./DataManagerTests/DataManagerTests.csproj
COPY ./DataManager/DataManager.csproj ./DataManager/DataManager.csproj

RUN dotnet restore DataManagerTests/DataManagerTests.csproj

COPY . .

CMD ["dotnet", "test", "DataManagerTests/DataManagerTests.csproj"] #, "--no-restore"]