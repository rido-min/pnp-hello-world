az extension add --name azure-iot
az login
az account set -s IOTPNP_TEST_BY_MAIN
az iot hub create --resource-group BugBash --sku S1 --location centraluseuap --partition-count 4 --name pnp-smr-01
az iot hub show-connection-string --name pnp-smr-01