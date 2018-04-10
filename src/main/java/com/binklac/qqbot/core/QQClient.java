package com.binklac.qqbot.core;

import io.netty.bootstrap.Bootstrap;
import io.netty.channel.Channel;
import io.netty.channel.ChannelOption;
import io.netty.channel.EventLoopGroup;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.DatagramPacket;
import io.netty.channel.socket.nio.NioDatagramChannel;

import java.net.InetSocketAddress;

public class QQClient {
    public void run(int port) {
        EventLoopGroup group  = new NioEventLoopGroup();
        try {
            Bootstrap b = new Bootstrap();
            b.group(group).channel(NioDatagramChannel.class)
                    .handler(new QQPacketHandler());
            Channel ch = b.bind(0).sync().channel();
            //向网段内的所有机器广播UDP消息。
            ch.writeAndFlush(new DatagramPacket(null, new InetSocketAddress("255.255.255.255",port))).sync();
            if(!ch.closeFuture().await(15000)){
                System.out.println("查询超时！");
            }
        } catch (Exception e) {
            group.shutdownGracefully();
        }
    }
}
