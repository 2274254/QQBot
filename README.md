# QQBot
  A simple QQ robot written in Java
  
  一个炒鸡简单的QQ机器人,使用Java编写
  
  现在还是个半成品,完整的会在这两天写完[flag]

### 两分钟极简入门教程
  这是一个基于消息与插件的机器人,想要实现自己的消息处理很简单,只需要编写一个插件
  
  首先,我们编写一个消息处理类
  ```java
  import com.binklac.qqbot.event.qqbotevents.QQBotLoginEvent;
  import com.binklac.qqbot.event.EventHandler;
  
  public class DemoPluginEventHandler {
      @EventHandler
      public void someFuckingFunction(QQBotLoginEvent Event){
          System.out.println(Event.getQQNumber());
      }
  }
  ```
  
  然后再
  
  ```java
    QQBot.instance().registerEventHandler(new DemoPluginEventHandler());
  ```
   就可以愉快的监听消息了
### 致谢
  该项目的关于处理QQWeb消息的代码很多来自
  
  https://github.com/ScienJus/smartqq
  
  部分算法参考了
  
  https://github.com/pandolia/qqbot
  
  Json解析使用了
  
  https://github.com/alibaba/fastjson