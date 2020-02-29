<img src="sources/frontend/src/assets/logo-horizontal.svg" width="500px"/>

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/ddadaal/AcademyCloud/frontend%20build%20and%20deploy?label=frontend%20build%20and%20deploy&style=flat-square)

AcademyCloud is a public IaaS cloud that is:

- designed for educational institutions
- built on OpenStack
- also my undergraduate graduation project

Check the demo version now! https://academycloud.ddadaal.me

[Opening Proposal](docs/research-proposal.md)

# Contents

- [reports](/reports)
  - daily and periodic reports
- [preparation](/preparation)
  - preparation works
- [openstack](/openstack)
  - OpenStack dpeloyment scripts
- [documentations](/docs)
  - [important talks between me and mentor](docs/important-talks.md)
- [sources](/sources)
  - The source code for the project, including
    - [backend](sources/backend)
      - Microservices communicating with each other by `gRPC`
      - `C# ASP.NET Core 3.1` used in 2 microservices + 1 API Gateway
      - `Python` used in 1 microservices
      - `consul` for service registration and discovery
      - `registrator` to automatically register containers when up and de-register when down
      - `docker-compose` for dev environment 
    - [frontend](sources/frontend)
      - `React` + `TypeScript` + `ant-design`
      - complete mock data for backend-less development
      - self-made [simstate](https://github.com/ddadaal/simstate) for state management
      - self-made **i18n solution** based on simstate
      - **All hooks** projects with no use of class components whatsoever

## License

Nothing from this project shall be used until my graduation.
