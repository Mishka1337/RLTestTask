export default function jwtHeader() {
    let token = JSON.parse(localStorage.getItem('token'));

    if (token) {
        return {
            headers: {
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + token.accessToken
            }
        }
    } else {
        return {
            headers: {
                'Content-Type': 'application/json'
            }
        }
    }
}