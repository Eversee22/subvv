import base64
import os,sys
import json
import argparse
import requests

def arg_parser():
    parser = argparse.ArgumentParser(description='get v2ray subscriptions')
    parser.add_argument('-url',dest='url',help='the url of subscriptions')
    parser.add_argument('-i',dest='ib64',default='inputb64',
        help='the input file of base64 coding of subscriptions')
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
        out=r.text
    except Exception:
        print('Url Error')
else:
    try:
        with open(ib64) as f:
            out=f.readline().strip()
    except Exception as e:
        print('File Error\n',e)
        sys.exit(1)
    
pre='dm1lc3M6'
print('get b64',len(out))
if out.find(pre)==-1:
	print('Unsupport Encoding')
	sys.exit(1)	
vms=out.split(pre)
vms=[v for v in vms if len(v)>0]
allink=[]
for v in vms:
	try:
		allink.append(base64.b64decode(v))
	except:
		pass
cnt=0
f=open(outf,'w')
for i,l in enumerate(allink):
    try:
        l=l[2:]
        lks=l.split(b'vmess://')
        for lk in lks:
            conf=base64.b64decode(lk)
            cnt+=1
            try:
                conf=json.loads(conf)
                strr='[%d]\nserver: %s\nport: %s\nid: %s\nalterId: %s\nnet: %s\ntls: %s\npath: %s\nadd: %s\nps: %s\n'%(
                    cnt,conf['add'],conf['port'],conf['id'],conf['aid'],conf['net'],conf['tls'],conf['path'],conf['add'],conf['ps'])
            except Exception:
                strr='[%d]\n%s'%(cnt,conf.decode('utf-8'))
            print(strr)
            f.write(strr+'\n')
    except:
        pass
f.close()
	