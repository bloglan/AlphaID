# 实名认证

## 概述

实名认证是指通过与自然人个体关联的、由权威机构签发的证明材料来证明自然人姓名等信息的真实性和有效性。

实名认证在某些企业应用中较为重要，能够明确建立起账户身份与自然人个体之间的明确关系，有助于溯源和人力资源管理。

截至目前，实名认证特性尚未完全使用于GDPR，我们正在努力改进。

## 实现

实名认证功能特性是一个扩展特性，你可以在系统中打开或关闭这一功能特性。

实名认证功能被设计为可扩展的，您可以在此基础上添加多种实名认证手段和证明材料种类。

### 实名认证管理

实名认证管理的核心是RealNameManager。每个自然人的实名认证信息用 RealNameAuthentication 表示，每个自然人可以有多个 RealNameAuthentication 。

RealNameAuthentication 本身不会去验证个人的真实性和有效性，这项工作应该在创建 RealNameAuthentication 之前就完成了。为用户创建 Authentication 时，意味着组成 Authentication 的信息是已经被认可实名的。个人用户只要持有任意一个尚处于有效期内的 RealNameAuthentication 时，即被认为已通过实名认证。

个人实名信息的真实性和有效性验证由 RealNameRequestManager 来实施，详见下文。鉴于实名认证的复杂多样性，您也可以给予您的条件和业务规则，编写适合您组织的实名信息审核业务逻辑。

### 个人信息更改约束

用户通过实名认证后，某些个人信息（如姓名、性别等）就不再允许更改。为实现这一约束，实名认证扩展会在更新个人信息时进行检查并应用个人信息更改约束。

如果启用了实名认证扩展，它会插入一个RealNameInterceptor拦截器，在更新用户时执行检查，阻止不允许的更改。

> 拦截器是 Alpha ID 的一种程序机制，允许在某些关键业务环节执行额外自定义操作或拦截更改操作行为。详见[拦截器]()。