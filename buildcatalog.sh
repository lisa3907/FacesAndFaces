cd ./src/Services/ProductCatalogApi
docker build --tag tkousek/catalog .
docker push tkousek/catalog
kubectl delete deployment catalog
kubectl delete service catalog
cd /Users/tkousek/AspNetCoreMicro/ShoesOnContainers/kompose
kubectl apply -f catalog-deployment.yaml
kubectl apply -f catalog-service.yaml
