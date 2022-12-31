cd ./src/Services/OrderApi
docker build --tag tkousek/order .
docker push tkousek/order
kubectl delete deployment order
kubectl delete service order
cd /Users/tkousek/AspNetCoreMicro/ShoesOnContainers/kompose
kubectl apply -f order-deployment.yaml
kubectl apply -f order-service.yaml
