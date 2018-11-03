# Dotnet Semantic Release

Inspired by the [Semantic Release][0] module for NodeJS, the intent is to completely remove manual intervention from the versioning process by automating the [Semantic Versioning][1] specification during build/release time.

**Note**: This package is still in a very early phase of development and not in NuGet yet.  Pull requests are welcome.

## Usage

### Global Tool

```sh
# Install tool
dotnet tool install --global SemanticRelease.GlobalTool

# View help
semantic-release --help

# Version and tag application
semantic-release release --project-path <path-to-csproj>
```

### Dotnet CLI Tool

Add the following to your `.csproj` file:

```xml
  <ItemGroup>
    <DotNetCliToolReference Include="SemanticRelease.Tool" Version="1.3.0" />
  </ItemGroup>
```

```sh
# Navigate to the *.csproj location
cd <project folder>

# Version and tag application
dotnet-semanticrelease release
```

### Standardizing commit messages

[Commitzen][2] is a great NodeJS tool that prompts developers at the time of commit for information regarding commit scope, description, issue id, etc.  It uses this information to form a commit message that follows a consistent format and makes it easy for tools like this to intepret.

dotnet-semantic-release currently relies on the `cz-convential-changelog` library for parsing commit messages.

To use it, install the latest version of NodeJS, NPM, and Conventional changelog library.

```sh
# Install commitizen
npm install -g commitizen

# Install conventional-changelog library
npm install -g cz-conventional-changelog

# Set the default changelog library
echo '{ "path": "cz-conventional-changelog" }' > ~/.czrc
```

From here on out, execute `npx git-cz` to perform commits.

## How does it work?

The tool checks for keywords inside commit messages to identify if the changes that occured should cause a version bump.  

For example:

* `feat`:  will bump the *minor* version number (ie. 1.**4**.3 would become 1.**5**.0).
* `fix`: will bump the patch version (ie. 1.27.**0** becomes 1.27.**1**).
* `Breaking Change`: will cause the major version to increment (ie. **2**.4.1 would become **3**.0.0).


## What happens when I "release"?

* Locate the nearest `.csproj` file and read the `Version`.  If `Version` doesn't exis, try to read `PackageVersion`.
* Verify that the current branch is the configured release branch, it has a remote source, and the current branch is up to date with the remote source
* Calculate the next version
* Update the `Version` and `PackageVersion` (if applicable)
* Tag the current branch

## Limitations

Here are a list of current limitations that will be fixed/implemented in future release of the library.  The list is in no particular order.

* Automatic changelog generation not implemented yet
* Extensibility model is unstable
* Only GIT is supported
* Tags are not automatically committed
* `Version` and `PackageVersion` are considered synonymous when locating an existing version.
* `VersionPrefix` and `VersionSuffix` are not considered


[0]:https://github.com/semantic-release/semantic-release
[1]:https://semver.org/
[2]:https://github.com/commitizen/cz-cli