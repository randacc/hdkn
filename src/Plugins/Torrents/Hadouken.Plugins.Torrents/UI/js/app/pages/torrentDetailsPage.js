﻿define(['rpcClient', 'eventListener', 'page'], function(RpcClient, EventListener, Page) {
    function TorrentDetailsPage() {
        Page.call(this, '/plugins/core.torrents/details.html', '/torrents/{id}');

        this.rpc = new RpcClient();
        this.eventListener = new EventListener();
    }

    TorrentDetailsPage.prototype = new Page();
    TorrentDetailsPage.prototype.constructor = TorrentDetailsPage;

    TorrentDetailsPage.prototype.load = function(id) {
        console.log(id);
    };

    TorrentDetailsPage.prototype.unload = function() {
        console.log('unloading details');
    };

    return TorrentDetailsPage;
});