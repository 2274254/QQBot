# QQBot
使用QQ PC协议的机器人, 实现简单的消息收发

***

### 简介
因为WebQQ无法获取好友的真实QQ,所以就去捣鼓了一下PC QQ的协议,顺便重学习一下C#  

这个Bot使用.net Core进行开发,理论上无需额外的修改即可跨平台兼容

***因为这是我第一天学C#写的东西,估计后续重构还是会有的***
***

### 目录
* [ToDo List](#ToDo)
* [插件编写](#插件编写)

### ToDo
* Readme
  - [ ] 完成Readme
* 插件系统
  - [x] 动态库的加载
  - [x] User Plugin的加载
  - [ ] User Plugin的卸载
  - [ ] Core Plugin的加载
* 网络处理
  - [x] 异步Socket
  - [ ] 登录/退出账号
* 消息系统
  - [ ] 群/好友的消息收发
  - [ ] 其它类型的消息处理
* Event系统
  - [ ] Event的注册/分发
  - [ ] Slow Event的处理

(可以看出来,坑基本没填)

### 插件编写
在一个C#库中,最多允许一个Core Plugin和一个User Plugin  
* Core Plugin
  * 接收传来的原始数据包并且阻止这个数据包发送给其他的Core Plugin
  * 可以直接发送,拦截,修改Event
  * 强制要求显式指定加载顺序
  * 出现异常可能波及其他的Plugin
  * 只能在启动时加载
  
* User Plugin
  * 只能接收被Event封装过的消息
  * 可以在运行时动态加载/卸载
  * 理论上抛出异常是不影响Bot工作的,嗯,理论上

##### User Plugin入口
特别特别简单,只需要按照以下形式稍微改改就行

```csharp
using System;
using QQBot.Plugin;

namespace QQBotCorePlugin {
    [Plugin("QQBotCorePlugin", "VeroFess", "coderzeng@hotmail.com", 0, 1)]
    public class QQBotCorePlugin : QQBotPlugin {
        public bool OnPluginLoad(PluginsManager manager) {
            Console.WriteLine("I'm loaded!!");
            return false;
        }

        public void OnPluginUnoad() {
            throw new NotImplementedException();
        }
    }
}
```



