import base64
import json
import requests


def parsedata(data):
    subs = []
    pre = 'dm1lc3M6'
    if data.find(pre) == -1:
        print('Not Supported Encoding')
        return subs
    datalen = len(data)
    print('length:', datalen)
    if len(data) % 4 != 0:
        data += '=' * ((datalen // 4 + 1) * 4 - datalen)
    try:
        data2 = base64.b64decode(data)
        data3 = data2.split(b'vmess://')
    except Exception as e:
        print('Decoding Error', e)
        return subs
    cnt = 0
    for i in data3:
        if len(i) > 0:
            cnt += 1
            try:
                t = base64.b64decode(i)
                try:
                    t = json.loads(t)
                    subs.append(t)
                    t2 = '[%d]\nserver: %s\nport: %s\nid: %s\nalterId: %s\n' \
                         'net: %s\ntls: %s\npath: %s\nhost: %s\nps: %s\n' \
                         % (cnt, t['add'], t['port'], t['id'], t['aid'],
                            t['net'], t['tls'], t['path'], t['host'], t['ps'])
                except Exception as e:
                    print("Json Read Error", e)
                    t2 = '[%d]\n' % cnt + str(t)
                print(t2)
            except Exception as e:
                print(cnt, "Decoding Error", e)
    return subs


def getsuburl(url, bkfile=None):
    print(url)
    try:
        r = requests.get(url)
        data = r.text
        if bkfile is not None:
            with open(bkfile, 'w') as f:
                f.write(data)
    except Exception as e:
        print('Url Error', e)
        return []
    return parsedata(data)


def getsubfile(filename):
    try:
        with open(filename) as f:
            data = f.read()
    except Exception as e:
        print('File Read Error', e)
        return []
    return parsedata(data)
