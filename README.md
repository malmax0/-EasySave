# EasySave [inDev]

## Description

What your application does,
Why you used the technologies you used,
Some of the challenges you faced and features you hope to implement in the future.

EasySave is a software allowing to manage data saves

&nbsp;

# Table of Contents
  - [Installation](#installation)
  - [Development](#development)
    - [Branch strategies](#branch-strategies)
    - [Naming convention](#naming-convention)
    - [Pull requests](#pull-requests)
  - [Credit](#credit)
  - [License](#license)

# Installation 

```bash
git clone
```

# Development
Want to contribute? Great!
## Branch strategies
![Branch Diagram](./assets/branchDiagram.png =80%x)

## Naming convention
[Microsoft c# convention](https://docs.microsoft.com/fr-fr/dotnet/csharp/fundamentals/coding-style/coding-conventions)

- PascalCasing for public, property, method, event, type, namespace, enum : `PublicField`
- Prefix by an underscore (and camelCasing) if it's a private field : `_privateField` 
- Prefix by an underscore (and PascalCasing) if it's a private static field : `_PrivateStaticField`
- camelCasing for local var : `myLocalVar`
- The const in MAJ + underscore : `MY_CONST`

## Pull requests
Pull requests are mandatory in our project, this involve we can't make changes directly in master branch (develop).

After you push or update a feature branch, Azure Repos displays a prompt to create a Pull Request.

![PR tuto 1/3](./assets/pullRequestTuto1.png =70%x)

![PR tuto 2/3](./assets/pullRequestTuto2.png =70%x)

When pull request is create you need wait the approval from reviewer to complete.
The reviewer can make comments on your code.

![PR tuto 3/3](./assets/pullRequestTuto3.png =100%x)

**Note :** If you have merge conflicts you can't complete pull request (significate branch develop is above your branch) try :

Merge develop in your branch or Rebase your branch on develop
# How to use ?

# Credit

_Contributors :_
  - MOIROUD Maxime
  - PAPY Lisa
  - CROUZET Sébastien
  - LAVERGNE Jules
  - CALIMACHE Paul
  
# License
MIT License : [View License](https://choosealicense.com/licenses/mit/)