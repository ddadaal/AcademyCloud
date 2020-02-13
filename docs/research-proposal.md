# 开题报告

南京大学软件学院 本科2016级 陈俊达

指导老师：
- 北京大学计算中心 樊春 高级工程师
- 南京大学软件学院 刘钦 讲师

题目：基于OpenStack的教学云资源管理系统的设计和实现

# 研究背景和意义

云计算是一种分布式计算模式，它能让用户通过网络快速地获取和访问共享的计算资源（包括网络、服务器、存储、应用和服务等），只用付出很少的管理成本。云平台从部署模式上可以分成公有云（可以由所有人使用）和私有云（仅供一个组织内部使用），从服务模式上主要分成软件即服务（Software as a Service，SaaS，提供可通过互联网访问的软件产品，如Google Apps）、平台即服务（Platform as a Service，PaaS，用户使用由提供商提供的编程语言、库、服务等编写应用软件并部署，如Heroku）和基础设施即服务（Infrastructure as a Service, IaaS，提供商提供部署好的网络、存储、处理器等基础设施，由用户自行决定其上运行的操作系统、软件等，如DigitalOcean） (Peter Mell, 2011)。在计费模式上，云服务经常采用只对使用的资源计费的模式。由于云计算的便利性和经济性，目前云服务已经在各个领域被广泛应用，公有云和私有云市场增长强劲，云服务在市场上已经被广泛接受。

随着教育机构用户对云资源的需求的逐渐增加，以及教育机构对信息化建设的越加重视，云平台在教育机构的作用和重要性逐渐凸显。使用云平台进行教育机构网络基础设施建设也能够帮助高校降低在复杂的软件硬件配置管理上的成本，使高校更专注于研究和教学活动 (Marinela Mircea, 2010)。除此之外，云计算还可以帮助学生快速创建复杂的学习环境、支持远程学习、支撑计算密集的任务（如机器学习计算）、降低学生和学校云计算资源的成本等 (José A. González-Martínez Miguel L. Bote-Lorenzo, 2015)。

OpenStack是一个开源的云平台框架，能够管理大量的计算、存储、网络等资源，使用者通过系统提供的API快速访问各种计算资源 (OpenStack, 2020)。用户可以通过OpenStack提供的各种组件快速在硬件上搭建起一个完整可用的、可伸缩、功能多样的IaaS私有/公有云平台。OpenStack开源、可扩展、提供了完整的云平台解决方案等特性，使得其比较适合包括教育机构在内的很多无法承受完全自主独立建设和维护平台的机构使用。这些机构可以使用较低的成本就能搭建和维护一个完整的云平台，并且能够借助OpenStack的API，根据自己的独特的需求，对系统进行定制和扩展。

研究和扩展云计算以及OpenStack在教育领域中的应用可以满足教育机构用户对云资源的需求，帮助教育机构的信息化建设，降低教育机构和用户的资源成本和维护成本，提高研究和教学活动的效率。教育云平台也为探索更先进的教学研究模式提供了有力基础。

# 国内外研究现状

在教育领域中，已经有一些教育机构在机构内采用云计算的例子，并取得了传统自己建设自己维护更好的效果。Hochschule Furtwangen University的CloudAI项目是一个可用来支持课程教学中相关应用（例如学习Java中需要学生实际操作的Java Web框架）的私有云平台。相比于之前给每个学生小组分配真正地物理机，并让学生在这个物理机上进行实践，这个云平台地投入大幅降低了学校IT部门（IMZ）维护机器的时间和难度，更高效地利用现有硬件，也便于统一软件配置环境，降低学生的学习难度和老师的教学难度。IMZ同时也开了用于Servlet的PaaS平台和CollabSoft在线学习系统的SaaS平台，进一步帮助教学高效地进行 (Frank Doelitzscher, 2011)。The University of Westminster在2007年在学校内部采用了Google Apps云平台，在机构内提供免费的邮箱、消息、共享日历、云存储服务，并以此替换了原来学校自己管理和维护的电子邮箱系统。之后，学校发现更多学生更愿意使用云平台提供的存储空间而非容易丢失的USB磁盘，更愿意直接使用云平台提供的邮箱而非之前的不受欢迎的学校自己的电子邮箱系统，而且学校的花费也比之前自己维护降低了许多（从100万欧元降到了0） (Sultan, 2010)。NATO Education and Training Network, UNED等更多学校也在大学内部搭建和应用了IaaS, PaaS和/或SaaS云平台，以辅助日常工作和教学 (José A. González-Martínez Miguel L. Bote-Lorenzo, 2015)。

OpenStack也在云计算甚至更多领域中被广泛使用。中国移动内部运营了一个主要由530计算节点和400余个存储和网络节点组成的1000节点的生产云平台，并对各个组件的性能进行了充分的测试、实验和优化 (China Mobile, Intel, 2016)。T-Mobile正在部署一个基于OpenStack的虚拟核心分组网演进（virtual evolved packet core，EPC），以及众多OpenStack的程序来可视化网络功能。通过在网络功能虚拟化（Network Funtion Virutalization）上使用OpenStack，T-Mobile降低了部署成本，提高了跨功能跨组协作效率，提高了核心系统自动化水平，缩短了发布服务到市场上所花费的时间 (T-Mobile, 2018)。OpenStack在官网上也列出了在不同领域的不同公司应用OpenStack的成功案例，证明了OpenStack应用的广泛。

OpenStack在教育领域中也有一些应用。Government College of Engineering基于OpenStack设计并实现了一种构建于教育机构目前已经有的硬件上的云OpenStack平台（Cloud OpenStack in Education Organization），并将云平台和传统的、直接在裸机上使用VMWare等虚拟机软件的方法的性能进行对比，并发现云平台在执行速度、资源利用率等方面有比较大的优势。同时云平台还具有数据备份等传统方法不具有的功能。通过这些实验，Government College of Engineering认为使用云平台相对于传统方法更适合教育机构 (Nikhil Wagh, 2019)。

另外，目前有众多高校正在自己建设自己校级高性能计算平台，包括南京大学 (南京大学高性能计算中心, 2020)、北京大学 (北京大学高性能计算平台, 2020)、上海交通大学 (上海交通大学网络信息中心, 2020)等。通过这些校级高性能计算平台，高校内部的用户可以通过网络访问大量的计算资源，以支持科研中遇到的问题。从云计算的定义来说，这样的高性能计算平台也是一种类似私有云的云平台。这样的高性能计算平台的建设，证明了目前部分高校已经发现了一些用户对云资源的需求，并已经开始为满足这些需求开始了行动。

# 主要研究或解决的问题和拟采用的方法

目前学生和教职工对云资源的需求与日俱增。例如，软件工程学科的教学经常需要云平台将学生的工作部署到互联网进行学习、练习和测试。但是，大多数学校并没有提供可供教职工使用的这样的云平台，使得用户不得不使用公有云平台，这带来了以下几个问题：
1.	学生、教职工等用户需要学习公有云平台为了满足更多用户的需求而设计的比较复杂的功能，提高了用户的学习和使用成本，降低了学习和教学的效率
2.	每个课题组、每门课堂、每个学生和教职工可能使用不同的公有云平台，公有云平台的管理系统也没有为学校场景专门定制，这提高了学校、课堂组和课堂等主体的管理成本
3.	老师若使用阿里云等外部平台为学生、为项目组提供资源，则需要涉及交费、签合同、领取发票等操作，对老师来说也是非常麻烦耗时间的事情。

本设计将会设计和实现一种基于OpenStack的教学云平台资源管理系统。这个平台是一个基于OpenStack实现的IaaS的公有云平台，在这之外，根据学校的具体情况，提供了一个学校->项目->用户的三级管理体系，以及一个按分配资源为标准的计费收费系统。

从管理模式上看，学校->项目->用户的管理模式更适合教育机构场景。每个学校在系统上可以注册一个独立的账号并获得一定的云资源，学校可以像私有云一样管理学校内部的项目和用户，并将云资源分配给各个项目。项目可对应为课题组、课堂等概念，每个用户可以加入多个项目，并在项目中使用项目管理员分配给个人的资源进行正常教学科研活动。这样，学校、老师等管理员可以很方便地掌握学校、项目组、课堂等资源使用情况，并据此进行管理。

从计费收费模式上看，平台能够降低学校、教职工和学生的使用成本，也能将老师从繁杂的财务工作中解放出来。平台通过给学校分配的资源进行收费，学校根据对项目分配的资源对项目进行收费。这样，学校可以只支付资源的费用，不付出维护成本，即可获得类似私有云的服务；普通用户和项目管理员只需要支付少量的管理员和资源使用费用，可以有效利用学校提供的云资源组织和进行教学和科研活动。同时，学校内部各个用户之间的资金支付通过走学校内部的途径，不需要进行交费等复杂的工作，降低老师使用的时间成本。

另外，本云平台将会运行在教育网上，教育网和运行在普通公网的公有云相比，有以下几个显著的优势：
1.	在国内，教育网有非常好的对称双向带宽，上传下载带宽都很好
2.	在国内，教育网的IPv6支持也最完善，做得也是最好的
3.	国际上，教育网的Internet带宽非常好，尤其是和国外大学之间。方便研究数据的国际间以及国内之间传递。

所以，和普通公有云相比，这样一个平台既能降低学校的维护成本，也能提高学生和教职工进行教学和科研活动的效率。

# 引用

China Mobile, Intel. (2016). Performance Analysis and Tuning in China Mobile's Openstack Production Cloud. Retrieved from https://01.org/sites/default/files/performance_analysis_and_tuning_in_china_mobiles_openstack_production_cloud_2.pdf

Frank Doelitzscher, A. S. (2011, January). Private cloud for collaboration and e-Learning services: from IaaS to SaaS. Computing, pp. 23-42.
José A. González-Martínez Miguel L. Bote-Lorenzo, E. G.-S.-P. (2015, January). Cloud computing and education: A state-of-the-art survey. Computer & Education, pp. 132-151.

Marinela Mircea, A. I. (2010, January). Using Cloud Computing in Higher Education: A Strategy to Improve. IBIMA Publishing.

Nikhil Wagh, V. P. (2019, March-April). Implementation of Stable Private Cloud Using OpenStack with Virtual Machine Results. International Journal of Computer Engineering & Technology, pp. 258-269.

OpenStack. (2020, February). Build the future of Open Infrastructure. Retrieved from OpenStack: https://www.openstack.org

Peter Mell, T. G. (2011). The NIST Definition of Cloud. U.S. Department of Commerce.

Sultan, N. (2010, April). Cloud computing for education: A new dawn? International Journal of Information Management, pp. 109-116.

T-Mobile. (2018, April 19). Vancouver Superuser Award Nominee: T-Mobile. Retrieved from Openstack Superuser: https://superuser.openstack.org/articles/vancouver-superuser-award-nominee-t-mobile/

北京大学高性能计算平台. (2020年February月). 北京大学高性能计算平台. 检索来源: 北京大学高性能计算平台: http://hpc.pku.edu.cn/

南京大学高性能计算中心. (2020年January月). 南京大学高性能计算中心. 检索来源: 南京大学高性能计算中心: https://hpcc.nju.edu.cn/

上海交通大学网络信息中心. (2020年February月). 上海交通大学高性能计算中心. 检索来源: 上海交通大学高性能计算中心: https://hpc.sjtu.edu.cn/


