# Token Accessor

This is a small tool to let you fetch your Azure access token via a localhost service. This was created with postman pre-request scripts in mind.

## Postman pre-request script

### 1. Add this to your pre-request script

```javascript
const tokenRequest = {
    url: "https://localhost:7001/getToken",
    method: "POST",
    header: {
        "Content-Type": "application/json",
    },
    body: {
        mode: "raw",
        raw: JSON.stringify(({ scopes: [ "https://management.azure.com/.default" ]})),
    },
};

pm.sendRequest(tokenRequest, (error, response) => {
    if (error) {
        console.log(error);
        return;
    }

    let token = response.json().token;
    pm.variables.set("access_token", token);
});
```

### 2. Set authorization to "Bearer" and value `{{access_token}}`

### 3. Run this service

`dotnet run`
