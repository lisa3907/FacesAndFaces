cd ./src/Services/TokenServiceApi
docker build --tag tkousek/tokenserver .
docker push tkousek/tokenserver
kubectl delete deployment tokenserver
kubectl delete service tokenserver
cd /Users/tkousek/AspNetCoreMicro/ShoesOnContainers/kompose
kubectl apply -f tokenserver-deployment.yaml
kubectl apply -f tokenserver-service.yaml
