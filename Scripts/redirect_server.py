from mitmproxy import http

SERVER_HOST = 'YOUR_SERVER_HERE'
SERVER_PORT = 80

REWRITE_HOST_LIST = [
    'ba-jp-sdk.bluearchive.jp',
    'prod-gateway.bluearchiveyostar.com',
    'prod-game.bluearchiveyostar.com',
    # 'prod-notice.bluearchiveyostar.com',
    # 'prod-logcollector.bluearchiveyostar.com',
]

def request(flow: http.HTTPFlow) -> None:
    if flow.request.pretty_host.endswith('log.aliyuncs.com'):
        flow.kill()
        return
    if flow.request.pretty_host in REWRITE_HOST_LIST:
        flow.request.scheme = 'http'
        flow.request.host = SERVER_HOST
        flow.request.port = SERVER_PORT
        return
