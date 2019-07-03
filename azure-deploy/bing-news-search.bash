az cognitiveservices account create \
--kind cognitiveservices \
--l southcentralus \
--sku S0 \
-g xam-dev-summit-scus \
-n xam-dev-summit-cs-scus \
--yes

az cosmosdb create \
--n xam-dev-summit-cosmos-scus \
-g xam-dev-summit-scus \
--kind GlobalDocumentDB \
--no-wait

az cosmosdb database create \
-n xam-dev-summit-cosmos-scus \
-g xam-dev-summit-scus \
--db-name user-preferences

az cosmosdb collection create \
-c Info \
-d user-preferences \
--partition-key-path /PartitionKey \
-n xam-dev-summit-cosmos-scus \
-g xam-dev-summit-scus

az storage account create \
-n xdsstoragescus \
-g xam-dev-summit-scus \
-l southcentralus \
--sku Standard_LRS \
--kind StorageV2