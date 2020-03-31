import base64
import os,sys
import json
import argparse
import requests

def arg_parser():
    parser = argparse.ArgumentParser(description='get v2ray subscriptions')
    parser.add_argument('-url',dest='url',help='the url of subscriptions')
    parser.add_argument('-i',dest='ib64',default='inputb64',
        help='the input file of base64 encoding of subscriptions')
    parser.add_argument('-o',dest='outf',default='TEMP',help='the output file of subscriptions')
    
    return parser.parse_args()

args=arg_parser()
url=args.url
ib64=args.ib64
outf=args.outf

if url is not None:
    print(url)
    try:
        r=requests.get(url)
        data=r.text
    except Exception:
        print('Url Error')
        sys.exit(1)
else:
    try:
        with open(ib64) as f:
            data=f.read()
    except Exception as e:
        print('File Error\n',e)
        sys.exit(1)
    
pre='dm1lc3M6'
if data.find(pre)==-1:
	print('Not Supported Encoding')
	sys.exit(1)
    
lendata=len(data)
print('length:',lendata)
if len(data)%4!=0:
    data+='='*((lendata//4+1)*4-lendata)
    # print(len(data))
try:
    data2=base64.b64decode(data)
    data3=data2.split(b'vmess://')
except Exception:
    print('Decoding Error 1')
    sys.exit(1)

cnt=0
with open(outf,'w',encoding='utf-8') as f:
    for i in data3:
        if len(i)>0:
            cnt+=1
            try:
                t=base64.b64decode(i)
                try:
                    t=json.loads(t)
                    t2='[%d]\nserver: %s\nport: %s\nid: %s\nalterId: %s\n\
net: %s\ntls: %s\npath: %s\nhost: %s\nps: %s\n'\
                    % (cnt,t['add'],t['port'],t['id'],t['aid'],
                    t['net'],t['tls'],t['path'],t['host'],t['ps'])
                except Exception:
                    t2=t.decode('utf-8')
                print(t2)
                f.write(t2+'\n')
            except Exception as e:
                print(cnt,e)
                # print(cnt,i)
	