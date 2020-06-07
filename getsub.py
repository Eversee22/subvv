import base64
import json
import requests

DBG = False


def parsedata(data):
    schemedata = {}
    data = data.strip()      \
        .replace('\n', '')   \
        .replace('\r\n', '') \
        .replace('\r', '')   \
        .replace(' ','')
    
    datalen = len(data)
    print('length:', datalen)
    if len(data) % 4 != 0:
        data += '=' * (datalen + 4 - datalen%4)
    try:
        data2 = base64.urlsafe_b64decode(data)
        items = data2.split(b'\n')
    except Exception as e:
        print('Decoding Error', e)
        return schemedata
    cnt = 0
    for i in items:
        if len(i) > 0:
            cnt += 1
            p = i.find(b':')
            scheme = i[:p].decode().lower()
            t = i[p+3:]
            if scheme != "vmess":
                strt = t.decode("utf-8")
                print(scheme+"://"+strt)
                if scheme in schemedata:
                    schemedata[scheme].append(strt)
                else:
                    schemedata[scheme] = [strt]
                continue

            try:
                t = base64.b64decode(t)
                try:
                    t = json.loads(t)
                    if scheme in schemedata:
                        schemedata[scheme].append(t)
                    else:
                        schemedata[scheme] = [t]
                    t2 = '[%d]\nserver: %s\nport: %s\nid: %s\nalterId: %s\n' \
                         'net: %s\ntls: %s\npath: %s\nhost: %s\nps: %s\n' \
                         % (cnt, t['add'], t['port'], t['id'], t['aid'],
                            t['net'], t['tls'], t['path'], t['host'], t['ps'])
                except Exception as e:
                    print("Json Read Error", e)
                    t2 = '[%d]\n' % cnt + str(t)
                if DBG:
                    print(t2)
            except Exception as e:
                print("#%d" % cnt, "Decoding Error", e)
    return schemedata


def getsuburl(url, bkfile=None):
    print(url)
    try:
        r = requests.get(url)
        data = r.text
        if bkfile is not None:
            with open(bkfile, 'a') as f:
                f.write("%s,%s\n" % (url, data))
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


def getsubraw(data):
    return parsedata(data)
