// K6 Load testing
import http, { RequestBody } from 'k6/http';
import { check, sleep } from 'k6';


/**
 * Authenticate using OAuth against Azure Active Directory
 * @function
 * @param  {string} tenantId - Directory ID in Azure
 * @param  {string} clientId - Application ID in Azure
 * @param  {string} clientSecret - Can be obtained from https://docs.microsoft.com/en-us/azure/storage/common/storage-auth-aad-app#create-a-client-secret
 * @param  {string} resource - Either a resource ID (as string) or an object containing username and password
 */
export function authenticateUsingAzure(tenantId, clientId, clientSecret, resource) {
    let url;
    const requestBody = {
        client_id: clientId,
        client_secret: clientSecret,
    };

    const params = {
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
    };

    if (typeof resource == 'string') {
        url = `https://login.microsoftonline.com/${tenantId}/oauth2/token`;
        requestBody['grant_type'] = 'client_credentials';
        requestBody['resource'] = resource;
    }

    const response = http.post(url, requestBody, params);

    return response.json();
}

// Runs once per 
export function setup() {
    // Or client credentials authentication flow
    let clientAuthResp = authenticateUsingAzure(
        __ENV.AZURE_TENANT_ID, __ENV.AZURE_CLIENT_ID, __ENV.AZURE_CLIENT_SECRET, __ENV.RESOURCE
    );

    return clientAuthResp;

}

export const options = {
    stages: [
        { target: 20, duration: '1m' },
        { target: 15, duration: '1m' },
        { target: 0, duration: '1m' },
    ],
    thresholds: {
        http_reqs: ['count < 100'],
    },
};

export default function (clientAuthResp) {
    const params = {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${clientAuthResp.access_token}`
        },
    };
    const getProductsApi = 'https://localhost:7109/api/products';

    const res = http.get(getProductsApi, params);

    check(res, {
        'response code was 200': (res) => res.status == 200,
    });

    sleep(1);
}
