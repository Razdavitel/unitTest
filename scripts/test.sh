cp -r /dotnet/tests/* tempdir/tests/.

cat > tempdir/Dockerfile << _EOF_
COPY ./src/Client/Squads.Client.csproj /src/Client/
COPY ./src/Domain/Domain.csproj /src/Domain/
COPY ./src/Persistence/Persistence.csproj /src/Persistence/
COPY ./src/Server/Squads.Server.csproj /src/Server/
COPY ./src/Services/Services.csproj /src/Services/
COPY ./src/Shared/Squads.Shared.csproj /src/Shared/
COPY ./tests/Domain.Tests/Domain.Tests.csproj /tests/Domain.Tests/

WORKDIR /src

# Restore dependencies
RUN dotnet restore "Server/Squads.Server.csproj"
RUN dotnet restore "/tests/Domain.Tests/Domain.Tests.csproj"


# Copy the rest of the files
COPY ./src /src
COPY ./tests /tests

CMD dotnet test /tests/Domain.Tests/Domain.Tests.csproj
_EOF_

cd tempdir || exit
docker build -t tests .
docker run tests
